-- Test Users for Online Ticket Platform
-- Insert test users with different roles

-- Admin User
INSERT INTO users (id, email, password_hash, name, phone, role, is_verified, registered_at, created_at)
VALUES (seq_users.NEXTVAL, 'admin@test.com', 'admin123', 'Admin User', '0123456789', 'admin', 1, SYSDATE, SYSDATE);

-- Organizer User
INSERT INTO users (id, email, password_hash, name, phone, role, is_verified, registered_at, created_at)
VALUES (seq_users.NEXTVAL, 'organizer@test.com', 'organizer123', 'Organizer User', '0987654321', 'organizer', 1, SYSDATE, SYSDATE);

-- Regular User
INSERT INTO users (id, email, password_hash, name, phone, role, is_verified, registered_at, created_at)
VALUES (seq_users.NEXTVAL, 'user@test.com', 'user123', 'Regular User', '0555666777', 'user', 1, SYSDATE, SYSDATE);

-- Guest User
INSERT INTO users (id, email, password_hash, name, phone, role, is_verified, registered_at, created_at)
VALUES (seq_users.NEXTVAL, 'guest@test.com', 'guest123', 'Guest User', '0111222333', 'guest', 1, SYSDATE, SYSDATE);

COMMIT; 