-- Tạo sequence cho image_id
CREATE SEQUENCE product_images_seq
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

-- Tạo trigger để tự động gán giá trị từ sequence vào image_id
CREATE OR REPLACE TRIGGER product_images_bir 
BEFORE INSERT ON productimages
FOR EACH ROW
BEGIN
    SELECT product_images_seq.NEXTVAL
    INTO :new.image_id
    FROM dual;
END;
/ 