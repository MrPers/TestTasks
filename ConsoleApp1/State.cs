namespace Calculator
{
    interface State
    {
        void digit(string key);
        void equal();
        void opers(string key);
        string scren();
    }
}
