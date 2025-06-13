-- Drop child tables first (avoid foreign key constraint errors)
DROP TABLE image_links CASCADE CONSTRAINTS;
DROP TABLE images CASCADE CONSTRAINTS;
DROP TABLE tracking_visits CASCADE CONSTRAINTS;
DROP TABLE webhook_subscriptions CASCADE CONSTRAINTS;
DROP TABLE webhook_logs CASCADE CONSTRAINTS;
DROP TABLE email_logs CASCADE CONSTRAINTS;
DROP TABLE referral_codes CASCADE CONSTRAINTS;
DROP TABLE order_promos CASCADE CONSTRAINTS;
DROP TABLE promo_codes CASCADE CONSTRAINTS;
DROP TABLE checkin_logs CASCADE CONSTRAINTS;
DROP TABLE payments CASCADE CONSTRAINTS;
DROP TABLE order_items CASCADE CONSTRAINTS;
DROP TABLE orders CASCADE CONSTRAINTS;
DROP TABLE tickets CASCADE CONSTRAINTS;
DROP TABLE event_settings CASCADE CONSTRAINTS;
DROP TABLE events CASCADE CONSTRAINTS;
DROP TABLE user_organizations CASCADE CONSTRAINTS;
DROP TABLE users CASCADE CONSTRAINTS;
DROP TABLE organizations CASCADE CONSTRAINTS;


DROP SEQUENCE seq_image_links;
DROP SEQUENCE seq_images;
DROP SEQUENCE seq_tracking_visits;
DROP SEQUENCE seq_webhook_subscriptions;
DROP SEQUENCE seq_webhook_logs;
DROP SEQUENCE seq_email_logs;
DROP SEQUENCE seq_referral_codes;
DROP SEQUENCE seq_order_promos;
DROP SEQUENCE seq_promo_codes;
DROP SEQUENCE seq_checkin_logs;
DROP SEQUENCE seq_payments;
DROP SEQUENCE seq_order_items;
DROP SEQUENCE seq_orders;
DROP SEQUENCE seq_tickets;
DROP SEQUENCE seq_event_settings;
DROP SEQUENCE seq_events;
DROP SEQUENCE seq_user_organizations;
DROP SEQUENCE seq_users;
DROP SEQUENCE seq_organizations;

