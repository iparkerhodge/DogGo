using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class DogRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT d.Id AS dId, d.[Name] AS dName, OwnerId, Breed, Notes, ImageUrl,
                            Email, o.[Name] AS oName, Address, Phone, NeighborhoodId, n.[Name] AS nName                           
                        FROM Dog d
                        JOIN Owner o ON o.id = d.OwnerId
                        JOIN Neighborhood n ON n.Id = o.NeighborhoodId
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        string notes = "";
                        string imageUrl = "";
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                        {
                            imageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("dId")),
                            Name = reader.GetString(reader.GetOrdinal("dName")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = notes,
                            ImageUrl = imageUrl,
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("oName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("nName"))
                                }
                            }
                        };

                        dogs.Add(dog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }

        public List<Dog> GetDogsByOwner(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], OwnerId, Breed, Notes, ImageUrl
                        FROM Dog
                        WHERE OwnerId = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while(reader.Read())
                    {
                        string notes = "";
                        string imageUrl = "";
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                        {
                            imageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = notes,
                            ImageUrl = imageUrl
                        };

                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT d.Id AS dId, d.[Name] AS dName, OwnerId, Breed, Notes, ImageUrl,
                            Email, o.[Name] AS oName, Address, Phone, NeighborhoodId, n.[Name] AS nName                           
                        FROM Dog d
                        JOIN Owner o ON o.id = d.OwnerId
                        JOIN Neighborhood n ON n.Id = o.NeighborhoodId
                        WHERE d.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string notes = "";
                        string imageUrl = "";
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                        {
                            imageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("dId")),
                            Name = reader.GetString(reader.GetOrdinal("dName")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = notes,
                            ImageUrl = imageUrl,
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("oName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                    Name = reader.GetString(reader.GetOrdinal("nName"))
                                }
                            }
                        };

                        reader.Close();
                        return dog;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Dog ([Name], Breed, Notes, ImageUrl, OwnerId)
                    OUTPUT INSERTED.Id
                    VALUES (@name, @breed, @notes, @imageUrl, @ownerId);
                ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    if(dog.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    }
                    if (dog.ImageUrl == null)
                    {
                        cmd.Parameters.AddWithValue("@imageUrl", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    }
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Dog
                            SET 
                                [Name] = @name, 
                                Breed = @breed, 
                                Notes = @notes, 
                                ImageUrl = @imageUrl, 
                                OwnerId = @ownerId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    if (dog.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    }
                    if (dog.ImageUrl == null)
                    {
                        cmd.Parameters.AddWithValue("@imageUrl", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    }
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDog(int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", dogId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
