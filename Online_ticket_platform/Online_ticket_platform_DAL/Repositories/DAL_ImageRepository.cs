using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Online_ticket_platform_Model;
using Online_ticket_platform_DAL.Interfaces;

namespace Online_ticket_platform_DAL.Repositories
{
    public class DAL_ImageRepository : DAL_IImageRepository
    {
        private readonly string _connectionString;

        public DAL_ImageRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public List<MOD_Image> GetAllImages()
        {
            var images = new List<MOD_Image>();
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = @"SELECT id, file_path, file_name, file_type, file_size, alt_text, uploaded_by, uploaded_at 
                               FROM images 
                               ORDER BY uploaded_at DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            images.Add(MapImageFromReader(reader));
                        }
                    }
                }
            }
            return images;
        }

        public MOD_Image GetImageById(int id)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = @"SELECT id, file_path, file_name, file_type, file_size, alt_text, uploaded_by, uploaded_at 
                               FROM images 
                               WHERE id = :id";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapImageFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public int InsertImage(MOD_Image image)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO images (
                                id, file_path, file_name, file_type, file_size, alt_text, uploaded_by, uploaded_at
                            ) VALUES (
                                seq_images.NEXTVAL, :file_path, :file_name, :file_type, :file_size, :alt_text, :uploaded_by, SYSTIMESTAMP
                            )
                            RETURNING id INTO :id";

                        using (var cmd = new OracleCommand(query, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.BindByName = true;
                            cmd.Parameters.Add(":file_path", OracleDbType.Varchar2).Value = image.FilePath;
                            cmd.Parameters.Add(":file_name", OracleDbType.Varchar2).Value = image.FileName ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":file_type", OracleDbType.Varchar2).Value = image.FileType ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":file_size", OracleDbType.Int32).Value = image.FileSize ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":alt_text", OracleDbType.Varchar2).Value = image.AltText ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":uploaded_by", OracleDbType.Int32).Value = image.UploadedBy ?? (object)DBNull.Value;

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
                        Console.WriteLine($"[InsertImage] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public bool UpdateImage(MOD_Image image)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE images 
                            SET file_path = :file_path,
                                file_name = :file_name,
                                file_type = :file_type,
                                file_size = :file_size,
                                alt_text = :alt_text,
                                uploaded_by = :uploaded_by
                            WHERE id = :id";

                        using (var cmd = new OracleCommand(query, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.BindByName = true;
                            cmd.Parameters.Add(":id", OracleDbType.Int32).Value = image.Id;
                            cmd.Parameters.Add(":file_path", OracleDbType.Varchar2).Value = image.FilePath;
                            cmd.Parameters.Add(":file_name", OracleDbType.Varchar2).Value = image.FileName ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":file_type", OracleDbType.Varchar2).Value = image.FileType ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":file_size", OracleDbType.Int32).Value = image.FileSize ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":alt_text", OracleDbType.Varchar2).Value = image.AltText ?? (object)DBNull.Value;
                            cmd.Parameters.Add(":uploaded_by", OracleDbType.Int32).Value = image.UploadedBy ?? (object)DBNull.Value;

                            int result = cmd.ExecuteNonQuery();
                            transaction.Commit();
                            return result > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[UpdateImage] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public bool DeleteImage(int id)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // First delete related image links
                        string deleteLinksQuery = "DELETE FROM image_links WHERE image_id = :image_id";
                        using (var cmd = new OracleCommand(deleteLinksQuery, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.Add(":image_id", OracleDbType.Int32).Value = id;
                            cmd.ExecuteNonQuery();
                        }

                        // Then delete the image
                        string deleteImageQuery = "DELETE FROM images WHERE id = :id";
                        using (var cmd = new OracleCommand(deleteImageQuery, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                            int result = cmd.ExecuteNonQuery();
                            transaction.Commit();
                            return result > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[DeleteImage] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public List<MOD_Image> GetImagesByUploader(int uploadedBy)
        {
            var images = new List<MOD_Image>();
            using (var conn = new OracleConnection(_connectionString))
            {
                string query = @"SELECT id, file_path, file_name, file_type, file_size, alt_text, uploaded_by, uploaded_at 
                               FROM images 
                               WHERE uploaded_by = :uploaded_by
                               ORDER BY uploaded_at DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":uploaded_by", OracleDbType.Int32).Value = uploadedBy;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            images.Add(MapImageFromReader(reader));
                        }
                    }
                }
            }
            return images;
        }

        private MOD_Image MapImageFromReader(OracleDataReader reader)
        {
            return new MOD_Image
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                FilePath = reader["file_path"] != DBNull.Value ? reader["file_path"].ToString() : string.Empty,
                FileName = reader["file_name"] != DBNull.Value ? reader["file_name"].ToString() : null,
                FileType = reader["file_type"] != DBNull.Value ? reader["file_type"].ToString() : null,
                FileSize = reader["file_size"] != DBNull.Value ? Convert.ToInt32(reader["file_size"]) : (int?)null,
                AltText = reader["alt_text"] != DBNull.Value ? reader["alt_text"].ToString() : null,
                UploadedBy = reader["uploaded_by"] != DBNull.Value ? Convert.ToInt32(reader["uploaded_by"]) : (int?)null,
                UploadedAt = reader["uploaded_at"] != DBNull.Value ? Convert.ToDateTime(reader["uploaded_at"]) : DateTime.MinValue
            };
        }
    }
} 