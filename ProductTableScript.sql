create TABLE Product (product_id int PRIMARY key, product_name varchar(20) not NULL,
category_id int, supplier_id int, unit_price NUMERIC(5,2), unit_in_stock int, discontinued BOOLEAN);

insert into Product (product_id, product_name, category_id, supplier_id, unit_price, unit_in_stock, discontinued) 
values (1,'Pen', 11, 21, 100, 10, '1');

select * from Product;
