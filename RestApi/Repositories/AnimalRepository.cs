using System.Data.SqlClient;
using RestApi.Domain;

namespace RestApi.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly string _connectionString;

    public AnimalRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Animal> FindAll()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Animal> animals = new List<Animal>();
            SqlCommand command = new SqlCommand(
                "SELECT * FROM Animals",
                connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    animals.Add(ReadEntity(reader));
                }
            }
            finally
            {
                reader.Close();
            }

            return animals;
        }
    }

    public Animal FindById(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(
                "SELECT * FROM Animals WHERE IdAnimal=@Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    return ReadEntity(reader);
                }
                else
                {
                    throw new NotFoundException();
                }
            }
            finally
            {
                reader.Close();
            }
        }
    }

    public void Add(Animal animal)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO Animals(IdAnimal, Name, Description, Category, Area)" +
                " VALUES(@Id, @Name, @Description, @Category, @Area)",
                connection);

            WriteEntity(animal, command.Parameters);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void Update(Animal animal)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(
                "UPDATE Animals SET Name=@Name, Description=@Description, Category=@Category, Area=@Area" +
                " WHERE IdAnimal=@Id",
                connection);

            WriteEntity(animal, command.Parameters);

            connection.Open();
            if (command.ExecuteNonQuery() == 0)
            {
                throw new NotFoundException();
            }
        }
    }

    public void Delete(Animal animal)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(
                "SELECT * FROM Animals WHERE IdAnimal=@Id AND Name=@Name" +
                " AND Description=@Description AND Category=@Category AND Area=@Area",
                connection);

            WriteEntity(animal, command.Parameters);

            connection.Open();
            if (command.ExecuteNonQuery() == 0)
            {
                throw new NotFoundException();
            }
        }
    }

    private Animal ReadEntity(SqlDataReader reader)
    {
        return new Animal(
            Convert.ToInt32(reader["IdAnimal"]),
            reader["Name"].ToString()!,
            reader["Category"].ToString()!,
            reader["Area"].ToString()!,
            reader["Description"].ToString());
    }

    private void WriteEntity(Animal animal, SqlParameterCollection parameters)
    {
        parameters.AddWithValue("@Id", animal.Id);
        parameters.AddWithValue("@Name", animal.Name);
        parameters.AddWithValue("@Category", animal.Category);
        parameters.AddWithValue("@Area", animal.Area);
        parameters.AddWithValue("@Description", animal.Description);
    }
}