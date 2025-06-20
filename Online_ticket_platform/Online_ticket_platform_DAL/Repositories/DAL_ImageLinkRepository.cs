using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Online_ticket_platform_Model;
using Online_ticket_platform_DAL.Interfaces;

namespace Online_ticket_platform_DAL.Repositories
{
    public class DAL_ImageLinkRepository : DAL_IImageLinkRepository
    {
        private readonly string _connectionString;

        public DAL_ImageLinkRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public List<MOD_ImageLink> GetAllImageLinks()
        {
            var imageLinks = new List<MOD_ImageLink>();
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT il.id, il.image_id, il.entity_type, il.entity_id, il.organization_id, il.event_id, 
                           il.usage_type, il.linked_at,
                           i.file_path, i.file_name, i.alt_text,
                           o.name as org_name,
                           e.name as event_name
                    FROM image_links il
                    LEFT JOIN images i ON il.image_id = i.id
                    LEFT JOIN organizations o ON il.organization_id = o.id
                    LEFT JOIN events e ON il.event_id = e.id
                    ORDER BY il.linked_at DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imageLinks.Add(MapImageLinkFromReader(reader));
                        }
                    }
                }
            }
            return imageLinks;
        }

        public MOD_ImageLink GetImageLinkById(int id)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT il.id, il.image_id, il.entity_type, il.entity_id, il.organization_id, il.event_id, 
                           il.usage_type, il.linked_at,
                           i.file_path, i.file_name, i.alt_text,
                           o.name as org_name,
                           e.name as event_name
                    FROM image_links il
                    LEFT JOIN images i ON il.image_id = i.id
                    LEFT JOIN organizations o ON il.organization_id = o.id
                    LEFT JOIN events e ON il.event_id = e.id
                    WHERE il.id = :id";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapImageLinkFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public List<MOD_ImageLink> GetImageLinksByEntity(string entityType, int entityId)
        {
            var imageLinks = new List<MOD_ImageLink>();
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT il.id, il.image_id, il.entity_type, il.entity_id, il.organization_id, il.event_id, 
                           il.usage_type, il.linked_at,
                           i.file_path, i.file_name, i.alt_text
                    FROM image_links il
                    LEFT JOIN images i ON il.image_id = i.id
                    WHERE il.entity_type = :entity_type AND il.entity_id = :entity_id
                    ORDER BY il.linked_at DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":entity_type", OracleDbType.Varchar2).Value = entityType;
                    cmd.Parameters.Add(":entity_id", OracleDbType.Int32).Value = entityId;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imageLinks.Add(MapImageLinkFromReader(reader));
                        }
                    }
                }
            }
            return imageLinks;
        }

        public List<MOD_ImageLink> GetImageLinksByEvent(int eventId)
        {
            var imageLinks = new List<MOD_ImageLink>();
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT il.id, il.image_id, il.entity_type, il.entity_id, il.organization_id, il.event_id, 
                           il.usage_type, il.linked_at,
                           i.file_path, i.file_name, i.alt_text
                    FROM image_links il
                    LEFT JOIN images i ON il.image_id = i.id
                    WHERE il.event_id = :event_id
                    ORDER BY il.linked_at DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":event_id", OracleDbType.Int32).Value = eventId;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imageLinks.Add(MapImageLinkFromReader(reader));
                        }
                    }
                }
            }
            return imageLinks;
        }

        public List<MOD_ImageLink> GetImageLinksByOrganization(int organizationId)
        {
            var imageLinks = new List<MOD_ImageLink>();
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT il.id, il.image_id, il.entity_type, il.entity_id, il.organization_id, il.event_id, 
                           il.usage_type, il.linked_at,
                           i.file_path, i.file_name, i.alt_text
                    FROM image_links il
                    LEFT JOIN images i ON il.image_id = i.id
                    WHERE il.organization_id = :organization_id
                    ORDER BY il.linked_at DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":organization_id", OracleDbType.Int32).Value = organizationId;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imageLinks.Add(MapImageLinkFromReader(reader));
                        }
                    }
                }
            }
            return imageLinks;
        }

        public int InsertImageLink(MOD_ImageLink imageLink)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO image_links (
                                id, image_id, entity_type, entity_id, organization_id, event_id, usage_type, linked_at
                            ) VALUES (
                                seq_image_links.NEXTVAL, :image_id, :entity_type, :entity_id, :organization_id, :event_id, :usage_type, SYSTIMESTAMP
                            )
                            RETURNING id INTO :id";

                        using (var cmd = new OracleCommand(query, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.Add(":image_id", OracleDbType.Int32).Value = imageLink.ImageId;
                            cmd.Parameters.Add(":entity_type", OracleDbType.Varchar2).Value = imageLink.EntityType;
                            cmd.Parameters.Add(":entity_id", OracleDbType.Int32).Value = imageLink.EntityId;
                            cmd.Parameters.Add(":organization_id", OracleDbType.Int32).Value = imageLink.OrganizationId ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":event_id", OracleDbType.Int32).Value = imageLink.EventId ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":usage_type", OracleDbType.Varchar2).Value = imageLink.UsageType;

                            var idParam = new OracleParameter(":id", OracleDbType.Int32)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(idParam);

                            cmd.ExecuteNonQuery();
                            int newId = Convert.ToInt32(idParam.Value.ToString());

                            transaction.Commit();
                            return newId;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[InsertImageLink] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public bool UpdateImageLink(MOD_ImageLink imageLink)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE image_links 
                            SET image_id = :image_id,
                                entity_type = :entity_type,
                                entity_id = :entity_id,
                                organization_id = :organization_id,
                                event_id = :event_id,
                                usage_type = :usage_type
                            WHERE id = :id";

                        using (var cmd = new OracleCommand(query, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.Add(":id", OracleDbType.Int32).Value = imageLink.Id;
                            cmd.Parameters.Add(":image_id", OracleDbType.Int32).Value = imageLink.ImageId;
                            cmd.Parameters.Add(":entity_type", OracleDbType.Varchar2).Value = imageLink.EntityType;
                            cmd.Parameters.Add(":entity_id", OracleDbType.Int32).Value = imageLink.EntityId;
                            cmd.Parameters.Add(":organization_id", OracleDbType.Int32).Value = imageLink.OrganizationId ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":event_id", OracleDbType.Int32).Value = imageLink.EventId ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":usage_type", OracleDbType.Varchar2).Value = imageLink.UsageType;

                            int result = cmd.ExecuteNonQuery();
                            transaction.Commit();
                            return result > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[UpdateImageLink] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public bool DeleteImageLink(int id)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = "DELETE FROM image_links WHERE id = :id";
                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public bool DeleteImageLinksByEntity(string entityType, int entityId)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = "DELETE FROM image_links WHERE entity_type = :entity_type AND entity_id = :entity_id";
                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":entity_type", OracleDbType.Varchar2).Value = entityType;
                    cmd.Parameters.Add(":entity_id", OracleDbType.Int32).Value = entityId;
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public List<MOD_Organization> GetAllOrganizationsForImageLink()
        {
            var organizations = new List<MOD_Organization>();
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = "SELECT id, name FROM organizations ORDER BY name";
                using (var cmd = new OracleCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            organizations.Add(new MOD_Organization
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString()
                            });
                        }
                    }
                }
            }
            return organizations;
        }

        public List<MOD_Event> GetAllEventsForImageLink()
        {
            var events = new List<MOD_Event>();
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = "SELECT id, name FROM events ORDER BY name";
                using (var cmd = new OracleCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new MOD_Event
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString()
                            });
                        }
                    }
                }
            }
            return events;
        }

        private MOD_ImageLink MapImageLinkFromReader(OracleDataReader reader)
        {
            var imageLink = new MOD_ImageLink
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                ImageId = reader["image_id"] != DBNull.Value ? Convert.ToInt32(reader["image_id"]) : 0,
                EntityType = reader["entity_type"] != DBNull.Value ? reader["entity_type"].ToString() : string.Empty,
                EntityId = reader["entity_id"] != DBNull.Value ? Convert.ToInt32(reader["entity_id"]) : 0,
                OrganizationId = reader["organization_id"] != DBNull.Value ? Convert.ToInt32(reader["organization_id"]) : (int?)null,
                EventId = reader["event_id"] != DBNull.Value ? Convert.ToInt32(reader["event_id"]) : (int?)null,
                UsageType = reader["usage_type"] != DBNull.Value ? reader["usage_type"].ToString() : string.Empty,
                LinkedAt = reader["linked_at"] != DBNull.Value ? Convert.ToDateTime(reader["linked_at"]) : DateTime.MinValue,
                Image = new MOD_Image
                {
                    Id = reader["image_id"] != DBNull.Value ? Convert.ToInt32(reader["image_id"]) : 0,
                    FilePath = reader["file_path"] != DBNull.Value ? reader["file_path"].ToString() : string.Empty,
                    FileName = reader["file_name"] != DBNull.Value ? reader["file_name"].ToString() : null,
                    AltText = reader["alt_text"] != DBNull.Value ? reader["alt_text"].ToString() : null
                }
            };

            // Add organization info if available
            if (reader["organization_id"] != DBNull.Value && reader["org_name"] != DBNull.Value)
            {
                imageLink.Organization = new MOD_Organization
                {
                    Id = Convert.ToInt32(reader["organization_id"]),
                    Name = reader["org_name"].ToString()
                };
            }

            // Add event info if available
            if (reader["event_id"] != DBNull.Value && reader["event_name"] != DBNull.Value)
            {
                imageLink.Event = new MOD_Event
                {
                    Id = Convert.ToInt32(reader["event_id"]),
                    Name = reader["event_name"].ToString()
                };
            }

            return imageLink;
        }
    }
} 