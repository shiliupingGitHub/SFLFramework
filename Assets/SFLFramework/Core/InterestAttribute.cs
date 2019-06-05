using System;

public class InterestAttribute : System.Attribute
{
    private Type _Interest;
    public Type Interest
    {
        get { return _Interest; }
    }
    public InterestAttribute(System.Type t)
    {
        _Interest = t;
    }
}
