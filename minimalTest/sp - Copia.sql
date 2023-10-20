CREATE PROCEDURE [dbo].[sp_BuscaTransaccion_Total]
    @desde AS SMALLDATETIME,
    @hasta AS SMALLDATETIME,
    @nombre AS VARCHAR(30),
    @Accion AS VARCHAR(8),
    @moneda AS CHAR(1),
    @Tarjeta AS VARCHAR(38),
    @OpcAdq AS INT = 0,
    @OpcMarca AS INT = 0,
    @Metodo AS INT = 0
AS

SET nocount ON
DECLARE @filtronombre BIT
DECLARE @TipoP1 CHAR(2)
DECLARE @TipoP2 CHAR(2)
DECLARE @TipoP3 CHAR(2)
DECLARE @Status1 CHAR(2)
DECLARE @Status2 CHAR(2)
DECLARE @Status3 CHAR(2)
DECLARE @Status4 CHAR(2)
DECLARE @timeout SMALLINT
DECLARE @NumTxn integer
DECLARE @NumPEN integer
DECLARE @NumUSD integer
DECLARE @TotPEN NUMERIC(18, 2)
DECLARE @TotUSD NUMERIC(18, 2)
DECLARE @Moneda1 CHAR(3)
DECLARE @Moneda2 CHAR(3)
/*Declarando 2 Variables para las nuevas fechas desde y hasta*/
DECLARE @FromDate AS DATETIME
DECLARE @ToDate AS DATETIME
/*Seteando las Variables @FromDate y @ToDate*/
SET @FromDate = CONVERT(DATETIME, @desde + '00:00:00')
SET @ToDate = CONVERT(DATETIME, @hasta + '23:59:59')
IF @moneda = 'A'
BEGIN
    SELECT @Moneda1 = '604'
    SELECT @Moneda2 = '840'
END
IF @moneda = 'S'
BEGIN
    SELECT @Moneda1 = '604'
    SELECT @Moneda2 = '604'
END
IF @moneda = 'D'
BEGIN
    SELECT @Moneda1 = '840'
    SELECT @Moneda2 = '840'
END


SELECT @filtronombre = 0
IF RTRIM(@nombre) <> ''
BEGIN
    SELECT @filtronombre = 1
END

--*** Se procesa @accion, puede ser "CONFIRMA", "REVERSA" O "DEVUELVE"            
IF @Accion = 'REVERSA'
BEGIN
    --declare @TipoProc1 char(2)            
    SELECT @TipoP1 = 'AT'
    SELECT @TipoP2 = 'CF'
    SELECT @TipoP3 = 'MA'
    SELECT @Status1 = ''
    SELECT @Status2 = ''
    SELECT @Status3 = ''
    SELECT @Status4 = 'CF'
    SELECT @timeout = CONVERT(SMALLINT, valor)
    FROM defaults WITH (NOLOCK)
    WHERE RTRIM(descripcion) = 'DiasVal_Aut_Devol'
END

--**** Genera Totales PEN             
SELECT @NumPEN = COUNT(a.IDTxn),
       @TotPEN = SUM(   CASE
                            WHEN a.TIPOPROC = 'AT'
                                 AND a.payMethodId <> 4 THEN
                                a.TXNAMOUNT                                                           -- AT
                            WHEN a.TIPOPROC = 'AT'
                                 AND a.payMethodId = 4 THEN
                                a.TXNAMOUNT + (CONVERT(NUMERIC(18, 2), c.TxnAmount * c.TipoDeCambio)) --AT MIXTA
                            WHEN c.TIPOPROC = 'MA'
                                 AND a.tipoproc <> 'AT' THEN
                                CONVERT(NUMERIC(18, 2), c.TxnAmount * c.TipoDeCambio)                 --SOLO MILLAS
                            ELSE
                                0
                        END
                    )
FROM Transacciones a WITH (NOLOCK, INDEX = IX_TRANSACCIONES03)
    INNER JOIN Direcciones b WITH (NOLOCK)
        ON ISNULL(a.IDTxnLink, a.IDTxn) = b.IDTxn
    INNER JOIN MARCA M WITH (NOLOCK)
        ON M.ID = A.ID_MARCA
    INNER JOIN PAYMENT.PAY_METHOD PM WITH (NOLOCK)
        ON PM.ID = CONVERT(INT, ISNULL(NULLIF(A.PAYMETHODID, ''), 1))
    LEFT JOIN TRANSACCIONES c WITH (NOLOCK)
        ON c.NroOC = a.NroOC
           AND c.TIPOPROC = 'MA'
           AND c.responseCode = '00'
WHERE (a.FechaTxn
      BETWEEN @FromDate AND @ToDate
      )
      AND
      (
          (
              a.tipoProc = @TipoP1
              AND a.StatusTxn = @Status1
          )
          OR
          (
              a.tipoProc = @TipoP2
              AND a.StatusTxn = @Status2
          )
          OR
          (
              a.tipoProc = @TipoP3
              AND a.StatusTxn = @Status3
              AND
              (
                  SELECT PayType
                  FROM Mile.Transacciones mt WITH (NOLOCK)
                  WHERE mt.NroOC = a.NROOC
                        AND mt.TipoProc = 'MA'
                        AND a.payMethodId = 4
                        AND mt.StatusTxn = @Status3
              ) = 1 --SOLO MILLAS
          )
      )
      AND DATEDIFF(day, a.FechaTxn, GETDATE()) <= @timeout
      AND
      (
          b.ApellidoBill LIKE '%' + @nombre + '%'
          OR @filtronombre = 0
      )
      AND a.TxnCurrency = '604'
      AND (a.AccountNumber LIKE '%' + RTRIM(LTRIM(@Tarjeta)) + '%')
      AND RTRIM(LTRIM(a.ResponseCode)) = '00'
      AND
      (
          @OpcAdq = 0
          OR A.ID_ADQ = @OpcAdq
      )
      AND
      (
          @OpcMarca = 0
          OR A.ID_MARCA = @OpcMarca
      )
      AND
      (
          @Metodo = 0
          OR ISNULL(NULLIF(A.PAYMETHODID, ''), '1') = @Metodo
      )

SELECT @NumUSD = COUNT(a.IDTxn),
       @TotUSD = SUM(   CASE
                            WHEN a.TIPOPROC = 'AT'
                                 AND a.payMethodId <> 4 THEN
                                a.TXNAMOUNT                                                           -- AT
                            WHEN a.TIPOPROC = 'AT'
                                 AND a.payMethodId = 4 THEN
                                a.TXNAMOUNT + (CONVERT(NUMERIC(18, 2), c.TxnAmount * c.TipoDeCambio)) --AT MIXTA
                            WHEN c.TIPOPROC = 'MA'
                                 AND a.tipoproc <> 'AT' THEN
                                CONVERT(NUMERIC(18, 2), c.TxnAmount * c.TipoDeCambio)                 --SOLO MILLAS
                            ELSE
                                0
                        END
                    )
FROM Transacciones a WITH (NOLOCK, INDEX = IX_TRANSACCIONES03)
    INNER JOIN Direcciones b WITH (NOLOCK)
        ON ISNULL(a.IDTxnLink, a.IDTxn) = b.IDTxn
    INNER JOIN MARCA M WITH (NOLOCK)
        ON M.ID = A.ID_MARCA
    INNER JOIN PAYMENT.PAY_METHOD PM WITH (NOLOCK)
        ON PM.ID = CONVERT(INT, ISNULL(NULLIF(A.PAYMETHODID, ''), 1))
    LEFT JOIN TRANSACCIONES c WITH (NOLOCK)
        ON c.NroOC = a.NroOC
           AND c.TIPOPROC = 'MA'
           AND c.responseCode = '00'
WHERE (a.FechaTxn
      BETWEEN @FromDate AND @ToDate
      )
      AND
      (
          (
              a.tipoProc = @TipoP1
              AND a.StatusTxn = @Status1
          )
          OR
          (
              a.tipoProc = @TipoP2
              AND a.StatusTxn = @Status2
          )
          OR
          (
              a.tipoProc = @TipoP3
              AND a.StatusTxn = @Status3
              AND
              (
                  SELECT PayType
                  FROM Mile.Transacciones mt WITH (NOLOCK)
                  WHERE mt.NroOC = a.NROOC
                        AND mt.TipoProc = 'MA'
                        AND a.payMethodId = 4
                        AND mt.StatusTxn = @Status3
              ) = 1 --SOLO MILLAS
          )
      )
      AND DATEDIFF(day, a.FechaTxn, GETDATE()) <= @timeout
      AND
      (
          b.ApellidoBill LIKE '%' + @nombre + '%'
          OR @filtronombre = 0
      )
      AND a.TxnCurrency = '840'
      AND (a.AccountNumber LIKE '%' + RTRIM(LTRIM(@Tarjeta)) + '%')
      AND RTRIM(LTRIM(a.ResponseCode)) = '00'
      AND
      (
          @OpcAdq = 0
          OR A.ID_ADQ = @OpcAdq
      )
      AND
      (
          @OpcMarca = 0
          OR A.ID_MARCA = @OpcMarca
      )
      AND
      (
          @Metodo = 0
          OR ISNULL(NULLIF(A.PAYMETHODID, ''), '1') = @Metodo
      )

IF @moneda = 'A'
    SELECT @NumTxn = @NumUSD + @NumPEN
IF @moneda = 'S'
    SELECT @NumTxn = @NumPEN
IF @moneda = 'D'
    SELECT @NumTxn = @NumUSD


--**** Genera Recordset            
SELECT @TipoP2 Tipo2,
       a.tipoProc TipoP2_tabla,
       a.transmissiondatetime 'fecha-hora',
       (b.apellidobill + ', ' + b.nombrebill) nombre,
       ISNULL(a.IDTxnLink, a.IDTxn) txn_id,
       'valor' = CASE
                     WHEN a.TIPOPROC = 'AT' THEN
       (a.TXNAMOUNT + ISNULL(CONVERT(NUMERIC(18, 2), c.TxnAmount * c.TipoDeCambio), 0))
                     WHEN a.TIPOPROC = 'MA' THEN
                         CONVERT(NUMERIC(18, 2), a.TXNAMOUNT * a.TipoDeCambio)
                     ELSE
                         a.TXNAMOUNT
                 END,
       CASE
           WHEN a.TxnCurrency = '840' THEN
               'USD'
           ELSE
               'PEN'
       END moneda,
       a.authIdResponse AuthId,
       a.accountNumber AcctNumb,
       @NumTxn TotTxn,
       @NumPEN NumPEN,
       @NumUSD NumUSD,
       @TotPEN TotPEN,
       @TotUSD TotUSD,
       a.datocuotas DatoCuotas,
       a.RespDatoCuota RespDatoCuota,
       a.CardAcceptorID merchant,
       a.TipoDeCambio TC,
       b.NroOC NroOC,
       M.DESCRIPCION AS MARCA,
       PM.Name AS Metodo,
       PM.ID AS PAYMETHODID,
       'TOTALMILLAS' = CASE
                           WHEN a.TIPOPROC = 'AT'
                                AND a.payMethodId = 4 THEN
                               ISNULL(CONVERT(INT, c.TxnAmount), 0)
                           ELSE
       (ISNULL(CONVERT(INT, a.TxnAmount), 0))
                       END,
       'TOTALCOMPRA' = CASE
                           WHEN a.TIPOPROC = 'AT'
                                AND a.PAYMETHODID = 4 THEN
       (a.TxnAmount - CONVERT(NUMERIC(18, 2), a.TxnAmount * a.TipoDeCambio))
                           ELSE
                               0
                       END,
       'MONTOMILLAS' = CASE
                           WHEN a.TIPOPROC = 'MA' THEN
                               ISNULL(CONVERT(NUMERIC(18, 2), a.TxnAmount * a.TipoDeCambio), 0)
                           WHEN a.TIPOPROC = 'AT'
                                AND a.PAYMETHODID = 4 THEN
                               ISNULL((CONVERT(NUMERIC(18, 2), c.TxnAmount * c.TipoDeCambio)), 0)
                           ELSE
                               0
                       END
FROM Transacciones a WITH (NOLOCK, INDEX = IX_TRANSACCIONES03)
    INNER JOIN Direcciones b WITH (NOLOCK)
        ON ISNULL(a.IDTxnLink, a.IDTxn) = b.IDTxn
    INNER JOIN MARCA M WITH (NOLOCK)
        ON M.ID = A.ID_MARCA
    INNER JOIN PAYMENT.PAY_METHOD PM WITH (NOLOCK)
        ON PM.ID = CONVERT(INT, ISNULL(NULLIF(A.PAYMETHODID, ''), 1))
    LEFT JOIN TRANSACCIONES c WITH (NOLOCK)
        ON c.NroOC = a.NroOC
           AND c.TIPOPROC = 'MA'
           AND c.responseCode = '00'
WHERE (a.FechaTxn
      BETWEEN @FromDate AND @ToDate
      )
      AND
      (
          (
              a.tipoProc = @TipoP1
              AND a.StatusTxn = @Status1
          )
          OR
          (
              a.tipoProc = @TipoP2
              AND a.StatusTxn = @Status2
          )
          OR
          (
              a.tipoProc = @TipoP3
              AND a.StatusTxn = @Status3
              AND
              (
                  SELECT PayType
                  FROM Mile.Transacciones mt WITH (NOLOCK)
                  WHERE mt.NroOC = a.NROOC
                        AND mt.TipoProc = 'MA'
                        AND a.payMethodId = 4
                        AND mt.StatusTxn = @Status3
              ) = 1 --SOLO MILLAS
          )
      )
      AND DATEDIFF(day, a.FechaTxn, GETDATE()) <= @timeout
      AND
      (
          b.ApellidoBill LIKE '%' + @nombre + '%'
          OR @filtronombre = 0
      )
      AND
      (
          a.TxnCurrency = @Moneda1
          OR a.TxnCurrency = @Moneda2
      )
      AND (a.AccountNumber LIKE '%' + RTRIM(LTRIM(@Tarjeta)) + '%')
      AND RTRIM(LTRIM(a.ResponseCode)) = '00'
      AND
      (
          @OpcAdq = 0
          OR A.ID_ADQ = @OpcAdq
      )
      AND
      (
          @OpcMarca = 0
          OR A.ID_MARCA = @OpcMarca
      )
      AND
      (
          @Metodo = 0
          OR ISNULL(NULLIF(A.PAYMETHODID, ''), '1') = @Metodo
      )
ORDER BY a.CardAcceptorID,
         a.IDTxn

SET NoCount OFF