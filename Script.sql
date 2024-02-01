CREATE TABLE Client (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(255),
    LastName NVARCHAR(255)
);

CREATE TABLE CreditCard (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Number NVARCHAR(255),
    HolderFirstName NVARCHAR(255),
    HolderLastName NVARCHAR(255),
    ClientId INT FOREIGN KEY REFERENCES Client(Id),
);

CREATE TABLE Payment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClientId INT FOREIGN KEY REFERENCES Client(Id),
    CardNumber NVARCHAR(255),
    PaymentDate NVARCHAR(255),
    Amount DECIMAL(18, 2)
)

CREATE TABLE Purchase (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClientId INT FOREIGN KEY REFERENCES Client(Id),
    CardNumber NVARCHAR(255)
    PurchaseDate NVARCHAR(255),
    Description NVARCHAR(255),
    Amount DECIMAL(18, 2),
    AuthorizationNumber NVARCHAR(255),
    Month NVARCHAR(255)
);
CREATE TABLE Statement (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClientId INT FOREIGN KEY REFERENCES Client(Id),
    CardNumber NVARCHAR(255),
    CurrentBalance DECIMAL(18, 2),
    CreditLimit DECIMAL(18, 2),
    BonifiableInterest DECIMAL(18, 2),
    AvailableBalance DECIMAL(18, 2)
);

CREATE TABLE TransactionInfo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClientId INT FOREIGN KEY REFERENCES Client(Id),
    CardNumber NVARCHAR(255),
    AuthorizationNumber NVARCHAR(255),
    Date DATETIME,
    Description NVARCHAR(255),
    Charge DECIMAL(18, 2)
);
CREATE TABLE TransactionPaymentInfo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClientId INT FOREIGN KEY REFERENCES Client(Id),
    CardNumber NVARCHAR(255),
    Date DATETIME,
    Description NVARCHAR(255),
    Payment DECIMAL(18, 2),
);

/*procedimientos almacenados*/

/*obtener los estados de cuenta */
CREATE PROCEDURE usp_GetAllStatements
AS
BEGIN
    SELECT
        Id,
        ClientId,
        CardNumber,
        CurrentBalance,
        CreditLimit,
        BonifiableInterest,
        AvailableBalance
    FROM
        Statement;
END

/*registrar un nuevo cliente*/
CREATE PROCEDURE sp_RegisterPurchase
    @ClientId INT,
    @CardNumber NVARCHAR(255),
    @PurchaseDate NVARCHAR(255),
    @Description NVARCHAR(255),
    @Amount DECIMAL(18, 2),
    @AuthorizationNumber NVARCHAR(255),
    @Month NVARCHAR(255)
AS
BEGIN


    INSERT INTO Purchase (ClientId, PurchaseDate, Description, Amount, AuthorizationNumber, Month)
    VALUES (@ClientId, @PurchaseDate, @Description, @Amount, @AuthorizationNumber, @Month);


    DECLARE @CurrentBalance DECIMAL(18, 2);
    SELECT @CurrentBalance = CurrentBalance FROM Statement WHERE ClientId = @ClientId AND CardNumber = @CardNumber;

    UPDATE Statement
    SET CurrentBalance = @CurrentBalance + @Amount,
        BonifiableInterest = (@CurrentBalance + @Amount) * (SELECT Top_Aux FROM CreditCard WHERE ClientId = @ClientId AND Number = @CardNumber),
        AvailableBalance = CreditLimit - (@CurrentBalance + @Amount)
    WHERE ClientId = @ClientId AND CardNumber = @CardNumber;

    INSERT INTO TransactionInfo (ClientId, CardNumber, AuthorizationNumber, Date, Description, Charge)
    VALUES (@ClientId, @CardNumber, @AuthorizationNumber, GETDATE(), @Description, @Amount);
END

/*obtener tarjetas de credito en base al numero de tarjeta */

CREATE PROCEDURE usp_SearchCreditCardByNumber
    @CardNumber NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        ClientId,
        Number,
        HolderFirstName,
        HolderLastName,
        Top_Aux
    FROM CreditCard
    WHERE Number LIKE '%' + @CardNumber + '%';
END;

/*obtener compras por cliente */

CREATE PROCEDURE GetPurchasesByClientId
    @ClientId INT
AS
BEGIN
    SELECT
        Id,
        PurchaseDate,
        Description,
        Amount,
        AuthorizationNumber,
        Month
    FROM
        Purchase
    WHERE
        ClientId = @ClientId;
END;

/*registrar una nueva compra */

CREATE PROCEDURE sp_RegisterPurchase
    @ClientId INT,
    @CardNumber NVARCHAR(255),
    @PurchaseDate NVARCHAR(255),
    @Description NVARCHAR(255),
    @Amount DECIMAL(18, 2),
    @AuthorizationNumber NVARCHAR(255),
    @Month NVARCHAR(255)
AS
BEGIN
    DECLARE @BonificableInterestPercentage DECIMAL(18, 2) = 0.25; -- Porcentaje de interés bonificable (ejemplo: 25%)

    INSERT INTO Purchase (ClientId,CardNumber, PurchaseDate, Description, Amount, AuthorizationNumber, Month)
    VALUES (@ClientId, @CardNumber, @PurchaseDate, @Description, @Amount, @AuthorizationNumber, @Month);

    DECLARE @CurrentBalance DECIMAL(18, 2);
    SELECT @CurrentBalance = CurrentBalance FROM Statement WHERE ClientId = @ClientId AND CardNumber = @CardNumber;

    -- Calcular el interés bonificable usando el porcentaje fijo
    DECLARE @BonificableInterest DECIMAL(18, 2) = @Amount * @BonificableInterestPercentage;

    UPDATE Statement
    SET CurrentBalance = @CurrentBalance + @Amount,
        BonifiableInterest = @BonificableInterest,
        AvailableBalance = CreditLimit - (@CurrentBalance + @Amount)
    WHERE ClientId = @ClientId AND CardNumber = @CardNumber;

    INSERT INTO TransactionInfo (ClientId, CardNumber, AuthorizationNumber, Date, Description, Charge)
    VALUES (@ClientId, @CardNumber, @AuthorizationNumber, GETDATE(), @Description, @Amount);
END;

/*registrar una nuevo pago */

CREATE PROCEDURE sp_RegisterPayment
    @ClientId INT,
    @CardNumber NVARCHAR(255),
    @PaymentDate DATETIME,
    @Amount DECIMAL(18, 2)
AS
BEGIN
    DECLARE @BonificableInterestPercentage DECIMAL(18, 2) = 0.25; -- Porcentaje de interés bonificable (ejemplo: 25%)

    -- Insertar el pago en la tabla Payment
    INSERT INTO Payment (ClientId, CardNumber, PaymentDate, Amount)
    VALUES (@ClientId, @CardNumber, @PaymentDate, @Amount);

    -- Actualizar el saldo y el historial de transacciones
    DECLARE @CurrentBalance DECIMAL(18, 2);
    SELECT @CurrentBalance = CurrentBalance FROM Statement WHERE ClientId = @ClientId AND CardNumber = @CardNumber;

    -- Calcular el interés bonificable usando el porcentaje fijo
    DECLARE @BonificableInterest DECIMAL(18, 2) = @Amount * @BonificableInterestPercentage;

    UPDATE Statement
    SET CurrentBalance = @CurrentBalance - @Amount,
        BonifiableInterest = @BonificableInterest,
        AvailableBalance = CreditLimit - (@CurrentBalance - @Amount)
    WHERE ClientId = @ClientId AND CardNumber = @CardNumber;

    -- Insertar la transacción en el historial de transacciones específico para pagos
    INSERT INTO TransactionPaymentInfo (ClientId, CardNumber, Date, Description, Payment)
    VALUES (@ClientId, @CardNumber, @PaymentDate, 'Pago', @Amount);
END;

/*obtener pagos por cliente */
CREATE PROCEDURE GetPaymentsByClientId
    @ClientId INT
AS
BEGIN
    SELECT
        Id,
        CardNumber,
        PaymentDate,
        Amount
    FROM
        Payment
    WHERE
        ClientId = @ClientId;
END;

/*obtener compras por cliente */
CREATE PROCEDURE GetPurchasesByClientId
    @ClientId INT
AS
BEGIN
    SELECT
        Id,
        ClientId,
        CardNumber,
        PurchaseDate,
        Description,
        Amount,
        AuthorizationNumber,
        Month
    FROM
        Purchase
    WHERE
        ClientId = @ClientId;
END;

/*obtener info de transacciones */

CREATE PROCEDURE GetTransactionInfoByClientId
    @ClientId INT
AS
BEGIN
    SELECT
        Id,
        ClientId,
        CardNumber,
        AuthorizationNumber,
        Date,
        Description,
        Charge
    FROM
        TransactionInfo
    WHERE
        ClientId = @ClientId;
END;