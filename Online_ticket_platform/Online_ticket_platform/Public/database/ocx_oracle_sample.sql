-- 1. Tổ chức: OCX Organization
INSERT INTO organizations (id, name, contact_email, phone, address, created_at)
VALUES (seq_organizations.NEXTVAL, 'OCX Organization', 'contact@ocx.vn', '0901234567', '123 Lê Lợi, Q.1, TP.HCM', SYSTIMESTAMP);

-- 2. Người dùng: Admin tổ chức
INSERT INTO users (id, email, password_hash, name, phone, role, is_verified, registered_at, created_at)
VALUES (seq_users.NEXTVAL, 'admin@ocx.vn', 'hashed_password_here', 'Nguyễn Văn A', '0908888999', 'org_admin', 1, SYSTIMESTAMP, SYSTIMESTAMP);

-- 3. Gán user cho organization
INSERT INTO user_organizations (id, user_id, organization_id, role)
VALUES (
  seq_user_organizations.NEXTVAL,
  (SELECT id FROM users WHERE email = 'admin@ocx.vn'),
  (SELECT id FROM organizations WHERE name = 'OCX Organization'),
  'owner'
);

-- 4. Sự kiện: OCX Concert 2025
INSERT INTO events (id, organization_id, name, description, event_date, location)
VALUES (
  seq_events.NEXTVAL,
  (SELECT id FROM organizations WHERE name = 'OCX Organization'),
  'OCX Concert 2025',
  'Đêm nhạc lớn nhất năm với các nghệ sĩ nổi bật của Indie Việt.',
  TO_DATE('2025-07-20', 'YYYY-MM-DD'),
  'Sân vận động Phú Thọ, TP.HCM'
);

-- 5. Cài đặt sự kiện
INSERT INTO event_settings (id, event_id, setting_key, setting_value)
VALUES (
  seq_event_settings.NEXTVAL,
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  'allow_guest_checkout',
  'true'
);

-- 6. Vé: Regular, VIP
INSERT INTO tickets (id, event_id, name, price, type, total, sold, start_sale_date, end_sale_date)
VALUES (
  seq_tickets.NEXTVAL,
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  'Vé Regular',
  300000,
  'Regular',
  1000,
  0,
  SYSTIMESTAMP,
  TO_TIMESTAMP('2025-07-19 23:59:59', 'YYYY-MM-DD HH24:MI:SS')
);

INSERT INTO tickets (id, event_id, name, price, type, total, sold, start_sale_date, end_sale_date)
VALUES (
  seq_tickets.NEXTVAL,
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  'Vé VIP',
  800000,
  'VIP',
  200,
  0,
  SYSTIMESTAMP,
  TO_TIMESTAMP('2025-07-19 23:59:59', 'YYYY-MM-DD HH24:MI:SS')
);

-- 7. Mã khuyến mãi
INSERT INTO promo_codes (id, code, description, discount_type, discount_value, max_uses, valid_from, valid_until)
VALUES (
  seq_promo_codes.NEXTVAL,
  'OCXEARLYBIRD',
  'Giảm 10% cho khách đặt sớm',
  'percent',
  10,
  200,
  SYSTIMESTAMP,
  TO_TIMESTAMP('2025-07-10 23:59:59', 'YYYY-MM-DD HH24:MI:SS')
);

-- 8. Khách đặt vé (user mới)
INSERT INTO users (id, email, password_hash, name, phone, role, is_verified, registered_at)
VALUES (
  seq_users.NEXTVAL,
  'guest01@example.com',
  NULL,
  'Trần Thị Khách',
  '0912345678',
  'guest',
  1,
  SYSTIMESTAMP
);

-- 9. Đơn hàng + Chi tiết đơn + Áp mã khuyến mãi
INSERT INTO orders (id, user_id, status, payment_method, amount)
VALUES (
  seq_orders.NEXTVAL,
  (SELECT id FROM users WHERE email = 'guest01@example.com'),
  'paid',
  'momo',
  270000
);

INSERT INTO order_items (id, order_id, ticket_id, quantity, price_snapshot)
VALUES (
  seq_order_items.NEXTVAL,
  (SELECT MAX(id) FROM orders),
  (SELECT id FROM tickets WHERE name = 'Vé Regular'),
  1,
  300000
);

INSERT INTO order_promos (id, order_id, promo_code_id, discount_applied)
VALUES (
  seq_order_promos.NEXTVAL,
  (SELECT MAX(id) FROM orders),
  (SELECT id FROM promo_codes WHERE code = 'OCXEARLYBIRD'),
  30000
);

-- 10. Log Email xác nhận
INSERT INTO email_logs (id, user_id, event_id, email_type, subject, status, sent_at)
VALUES (
  seq_email_logs.NEXTVAL,
  (SELECT id FROM users WHERE email = 'guest01@example.com'),
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  'order_confirmation',
  'Xác nhận đơn hàng OCX Concert',
  'sent',
  SYSTIMESTAMP
);

-- 11.
INSERT INTO referral_codes (id, user_id, code, created_at)
VALUES (
  seq_referral_codes.NEXTVAL,
  (SELECT id FROM users WHERE email = 'guest01@example.com'),
  'KHACHTRAN01',
  SYSTIMESTAMP
);

INSERT INTO webhook_subscriptions (id, organization_id, target_url, event_type)
VALUES (
  seq_webhook_subscriptions.NEXTVAL,
  (SELECT id FROM organizations WHERE name = 'OCX Organization'),
  'https://crm.ocx.vn/webhooks/payment',
  'payment_success'
);

INSERT INTO webhook_logs (
  id, target_url, event_type, order_id, event_id, user_id, payload, status_code, response_text, triggered_at
)
VALUES (
  seq_webhook_logs.NEXTVAL,
  'https://crm.ocx.vn/webhooks/payment',
  'payment_success',
  (SELECT MAX(id) FROM orders),
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  (SELECT id FROM users WHERE email = 'guest01@example.com'),
  '{"order_id": 123, "status": "paid"}',
  200,
  'OK',
  SYSTIMESTAMP
);


INSERT INTO checkin_logs (id, user_id, ticket_id, event_id, checkin_time, verified_by)
VALUES (
  seq_checkin_logs.NEXTVAL,
  (SELECT id FROM users WHERE email = 'guest01@example.com'),
  (SELECT id FROM tickets WHERE name = 'Vé Regular'),
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  SYSTIMESTAMP,
  'CheckinBot'
);


INSERT INTO tracking_visits (
  id, user_id, event_id, utm_source, utm_medium, utm_campaign, utm_content, referrer_url, landing_page
)
VALUES (
  seq_tracking_visits.NEXTVAL,
  (SELECT id FROM users WHERE email = 'guest01@example.com'),
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  'facebook',
  'cpc',
  'early_bird_push',
  'cta_mua_ve',
  'https://facebook.com/ad-ocx',
  'https://ocx.vn/events/ocx-concert-2025'
);


INSERT INTO images (id, file_path, file_name, file_type, file_size, alt_text, uploaded_by, uploaded_at)
VALUES (
  seq_images.NEXTVAL,
  '/uploads/events/banner_ocx2025.jpg',
  'banner_ocx2025.jpg',
  'image/jpeg',
  204800,
  'Banner OCX Concert 2025',
  (SELECT id FROM users WHERE email = 'admin@ocx.vn'),
  SYSTIMESTAMP
);

INSERT INTO image_links (
  id, image_id, entity_type, entity_id, organization_id, event_id, usage_type, linked_at
)
VALUES (
  seq_image_links.NEXTVAL,
  (SELECT MAX(id) FROM images),
  'event',
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  (SELECT id FROM organizations WHERE name = 'OCX Organization'),
  (SELECT id FROM events WHERE name = 'OCX Concert 2025'),
  'banner',
  SYSTIMESTAMP
);




