--
--
--
-- Drop previous versions of the tables if they exist, in reverse order of foreign keys.
DROP TABLE IF EXISTS Item;
DROP TABLE IF EXISTS Account;

CREATE TABLE Account (
    ID SERIAL PRIMARY KEY,
    EmailAddress varchar(50) NOT NULL,
    Name varchar(50),
    Password varchar(255) NOT NULL
);

CREATE TABLE Item (
    ID SERIAL PRIMARY KEY,
    OwnerAccount integer REFERENCES Account(ID),
    Name text,
    Description text,
    DatePosted timestamp DEFAULT CURRENT_TIMESTAMP
);

-- Allow Accounts to select data from the tables.
GRANT SELECT ON Account TO PUBLIC;
GRANT SELECT ON Item TO PUBLIC;

-- SAMPLE DATA ----------------------------------------------------------------
-- Insert data into Account
INSERT INTO Account (EmailAddress, Name, Password) VALUES
('alice@example.com', 'Alice Smith', 'password123'),
('bob@example.com', 'Bob Johnson', 'password456'),
('charlie@example.com', 'Charlie Brown', 'password789'),
('david@example.com', 'David Clark', 'password101'),
('ellen@example.com', 'Ellen White', 'password102'),
('frank@example.com', 'Frank Green', 'password103'),
('grace@example.com', 'Grace Lee', 'password104'),
('hannah@example.com', 'Hannah Black', 'password105');

-- Insert data into Item
INSERT INTO Item (OwnerAccount, Name, Description) VALUES
(1, 'Vintage Watch', 'A classic vintage watch from the 1960s.'),
(2, 'Mountain Bike', 'A high-performance mountain bike for off-road adventures.'),
(3, 'Guitar', 'An acoustic guitar in excellent condition.'),
(4, 'Smartphone', 'Latest model smartphone with all the features.'),
(5, 'Laptop', 'A powerful laptop for gaming and work.'),
(6, 'Camera', 'A DSLR camera with multiple lenses.'),
(7, 'Headphones', 'Noise-cancelling headphones for immersive sound.'),
(8, 'Bookshelf', 'A wooden bookshelf with multiple compartments.');