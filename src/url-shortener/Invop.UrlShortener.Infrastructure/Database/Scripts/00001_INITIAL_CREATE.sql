CREATE TABLE IF NOT EXISTS shortened_urls
(
    unique_code VARCHAR(11) PRIMARY KEY,
    long_url VARCHAR(2048) NOT NULL,
    created_by UUID NOT NULL,
    created_on TIMESTAMP WITH TIME ZONE NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_shortened_urls_created_by ON shortened_urls(created_by);