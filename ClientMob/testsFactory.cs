using System;
using System.Collections.Generic;
using System.Drawing;

public enum testTypes
{
    one,
    several,
    order
}


public class answer
{
    public string data { get; set; }
    public int order { get; set; }
    public override string ToString()
    {
        return (order + ": " + data);
    }

}

public class fullTest
{
    public string Name { get; set; }
    public List<test> testList { get; set; }
    public string comment { get; set; }
    public int countTests()
    {
        if (testList == null)
            return 0;
        return testList.Count;
    }
}

public class test
{
    public testTypes type { get; set; }
    public string question { get; set; }
    public Dictionary<int, answer> answers { get; set; }
    public List<int> rightAnswer { get; set; }
    public override string ToString()
    {
        return question;
    }
}
