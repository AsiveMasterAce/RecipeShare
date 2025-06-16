namespace RecipeApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            int a = 2;
            int b = 3;

            int result= a+ b;
            Assert.Equal(5, result);
        }
        [Fact]
        public void Test_MeantToFail()
        {
            int a = 9;
            int b = 3;

            int result= a+ b;
            Assert.Equal(5, result);
        }


    }
}