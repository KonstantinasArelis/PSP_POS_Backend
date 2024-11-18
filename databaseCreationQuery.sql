CREATE TABLE Tax (
    id INT,
    tax_name VARCHAR(255),
    tax_rate DECIMAL,
    is_valid BIT,
    PRIMARY KEY (id)
);

CREATE TABLE ProductType (
    TypeId INT,
    TypeName VARCHAR(20),
    PRIMARY KEY (TypeId)
);

INSERT INTO ProductType (TypeId, TypeName) VALUES
    (1, 'SERVICE'),
    (2, 'ITEM'),
    (3, 'SERVICE_CHARGE');

CREATE TABLE Business (
    id INT,
    business_name VARCHAR(255),
    business_address VARCHAR(255),
    phone VARCHAR(255),
    email VARCHAR(255),
    currency VARCHAR(255)
    PRIMARY KEY (id)
);

CREATE TABLE Category (
    id INT,
    business_id INT,
    sort_priority INT,
    category_name VARCHAR(255),
    PRIMARY KEY (id),
    FOREIGN KEY (business_id) REFERENCES Business(id)
);

CREATE TABLE Product (
    id INT,
    product_name VARCHAR(255),
    business_id INT,
    price DECIMAL,
    product_type INT,
    is_for_sale BIT,
    tax_id INT,
    category_id INT,
    can_discount_be_applied BIT,
    stock_quantity BIT,
    variations VARCHAR(1000),
    FOREIGN KEY (product_type) REFERENCES ProductType(TypeId),
    FOREIGN KEY (business_id) REFERENCES Business(id),
    FOREIGN KEY (tax_id) REFERENCES Tax(id),
    FOREIGN KEY (category_id) REFERENCES Category(id),
    PRIMARY KEY (id)
);

CREATE TABLE User_Role (
    role_id INT,
    role_name VARCHAR(20),
    PRIMARY KEY (role_id)
);

INSERT INTO User_Role (role_id, role_name) VALUES
    (1, 'OWNER'),
    (2, 'SUPER_ADMIN'),
    (3, 'EMPLOYEE');

CREATE TABLE AccountStatus (
    account_status_id INT,
    acount_status_name VARCHAR(20),
    PRIMARY KEY (account_status_id)
);

INSERT INTO AccountStatus (account_status_id, acount_status_name) VALUES
    (1, 'ACTIVE'),
    (2, 'INACTIVE'),
    (3, 'Left');

CREATE TABLE _User (
    id INT,
    business_id INT,
    user_name VARCHAR(255),
    user_username VARCHAR(255),
    User_Role INT,
    password_hash VARCHAR(255),
    tips_amount DECIMAL,
    last_withdrawn_timestamp DATETIME,
    AccountStatus INT,
    PRIMARY KEY (id),
    FOREIGN KEY (User_Role) REFERENCES User_Role(role_id),
    FOREIGN KEY (AccountStatus) REFERENCES AccountStatus(account_status_id),
    FOREIGN KEY (business_id) REFERENCES Business(id)
);

CREATE TABLE ServiceEmployee (
    employee_id INT,
    product_id INT,
    PRIMARY KEY (employee_id, product_id),
    FOREIGN KEY (employee_id) REFERENCES _User(id),
    FOREIGN KEY (product_id) REFERENCES Product(id)
);

CREATE TABLE ReservationStatus (
    reservation_status_id INT,
    reservation_status_name VARCHAR(255),
    PRIMARY KEY (reservation_status_id)
);

INSERT INTO ReservationStatus (reservation_status_id, reservation_status_name) VALUES
    (1, 'RESERVED'),
    (2, 'CANCELLED'),
    (3, 'DONE');

CREATE TABLE Reservation (
    id INT,
    business_id INT,
    employee_id INT,
    client_name VARCHAR(255),
    client_phone VARCHAR(255),
    created_at DATETIME,
    last_modified DATETIME,
    appointment_time DATETIME,
    duration INT,
    ReservationStatus INT,
    service_id INT,
    PRIMARY KEY (id),
    FOREIGN KEY (ReservationStatus) REFERENCES ReservationStatus(reservation_status_id),
    FOREIGN KEY (business_id) REFERENCES Business(id),
    FOREIGN KEY (employee_id) REFERENCES _User(id),
    FOREIGN KEY (service_id) REFERENCES Product(id)
);

CREATE TABLE DiscountType (
    discount_type_id INT,
    discount_type_name VARCHAR(20),
    PRIMARY KEY (discount_type_id) 
)

INSERT INTO DiscountType (discount_type_id, discount_type_name) VALUES
    (1, 'ORDER'),
    (2, 'ORDER_ITEM');

CREATE TABLE OrderStatus (
    order_status_id INT,
    order_status_name VARCHAR(20),
    PRIMARY KEY (order_status_id) 
)

INSERT INTO OrderStatus (order_status_id, order_status_name) VALUES
    (1, 'OPEN'),
    (2, 'PENDING_PAYMENT'),
    (3, 'CLOSED');

CREATE TABLE PaymentMethod (
    payment_method_id INT,
    payment_method_name VARCHAR(20),
    PRIMARY KEY (payment_method_id) 
)

INSERT INTO PaymentMethod (payment_method_id, payment_method_name) VALUES
    (1, 'CARD'),
    (2, 'CASH'),
    (3, 'GIFTCARD');

CREATE TABLE PaymentStatus (
    payment_status_id INT,
    payment_status_name VARCHAR(20),
    PRIMARY KEY (payment_status_id) 
)

INSERT INTO PaymentStatus (payment_status_id, payment_status_name) VALUES
    (1, 'IN_PROGRESS'),
    (2, 'REJECTED'),
    (3, 'DONE');

CREATE TABLE Discount (
    id INT,
    business_id INT,
    product_id INT,
    discount_type INT,
    amount DECIMAL(8,2),
    discount_percentage DECIMAL(5,2),
    valid_from DATETIME,
    valid_until DATETIME,
    code_hash VARCHAR(255),
    PRIMARY KEY (id),
    FOREIGN KEY (business_id) REFERENCES Business(id),
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (discount_type) REFERENCES DiscountType(id),
)

CREATE TABLE _Order (
    id INT,
    business_id INT,
    employee_id INT,
    order_discount_percentage DECIMAL,
    total_amount DECIMAL,
    total_discount_amount DECIMAL,
    order_status INT,
    created_at DATETIME,
    closed_at DATETIME,
    PRIMARY KEY (id),
    FOREIGN KEY (business_id) REFERENCES Business(id),
    FOREIGN KEY (employee_id) REFERENCES _User(id),
    FOREIGN KEY (order_status) REFERENCES OrderStatus(order_status_id),
)

CREATE TABLE OrderItem (
    id INT,
    order_id INT,
    product_id INT,
    reservation_id INT,
    quantity INT,
    variations VARCHAR(1000),
    product_name VARCHAR(255),
    product_price DECIMAL,
    tax_id INT,
    variation_price DECIMAL,
    item_discount_amount DECIMAL,
    PRIMARY KEY (id),
    FOREIGN KEY (order_id) REFERENCES _Order(id),
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (reservation_id) REFERENCES Reservation(id),
    FOREIGN KEY (tax_id) REFERENCES Tax(id),
)

CREATE TABLE Refund (
    id INT,
    business_id INT,
    order_item_id INT,
    returned_to_inventory BIT,
    refunded_quantity INT,
    amount DECIMAL,
    reason VARCHAR(255),
    created_at DATETIME,
    PRIMARY KEY (id),
    FOREIGN KEY (business_id) REFERENCES Business(id),
    FOREIGN KEY (order_item_id) REFERENCES OrderItem(id),
)

CREATE TABLE Payment (
    id INT,
    business_id INT,
    order_id INT,
    total_amount DECIMAL,
    order_amount DECIMAL,
    tip_amount DECIMAL,
    payment_method INT,
    created_at DATETIME,
    payment_status INT,
    gift_card_id INT,
    PRIMARY KEY (id),
    FOREIGN KEY (business_id) REFERENCES Business(id),
    FOREIGN KEY (order_id) REFERENCES _Order(id),
    FOREIGN KEY (payment_status) REFERENCES PaymentStatus(payment_status_id),
    FOREIGN KEY (payment_method) REFERENCES PaymentMethod(payment_method_id),
)

CREATE TABLE GiftCard (
    id INT,
    original_amount DECIMAL,
    amount_left DECIMAL,
    valid_from DATETIME,
    valid_until DATETIME,
    code_hash VARCHAR(255)
)