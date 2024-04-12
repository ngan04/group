using SIMS_Demo.Controllers;

namespace SIMS_TEST;

public class UnitTest1
{
    [Fact]
    public void TestReadJsonFileWith3Teachers()
    {
        //1. Arrange
        //2. Act
        var teachers = TeacherController.ReadFileToTeacherList("data.json");
        //3. Assert
        Assert.Equal(3, teachers.Count);
    }
}
