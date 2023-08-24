CREATE TABLE clients (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    name VARCHAR (100) NOT NULL,
    email VARCHAR (150) NOT NULL UNIQUE,
    phone VARCHAR(20) NULL,
    address VARCHAR(100) NULL,
    IdentityNumber VARCHAR(100) NULL
);
INSERT INTO clients (name, email, phone, address, IdentityNumber)
VALUES
('Bill Gates', 'bill.gates@microsoft.com', '+123456789', 'New York, USA','21531603652'),
('Elon Musk', 'elon.musk@spacex.com', '+111222333', 'Florida, USA','22985674328'),
('Will Smith', 'will.smith@gmail.com', '+111333555', 'California, USA','60071025562'),
('Bob Marley', 'bob@gmail.com', '+111555999', 'Texas, USA','24816331660'),
('Erkin Avci', 'erkin.avci@gmail.com', '+32447788993', 'Izmir, Turkey','80656393718'),
('Boris Johnson', 'boris.johnson@gmail.com', '+4499778855', 'London, England','99669473058');
