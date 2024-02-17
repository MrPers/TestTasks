namespace Calculator
{
    internal class StateFirst : State
    {
        private Context context;

        public StateFirst(Context context)
        {
            this.context = context;
            context.x = 0;
        }

        public void digit(string key)
        {
            int digit = Convert.ToInt32(key);
            context.x = context.x * 10 + digit;
        }

        public void equal()
        {
            context.ChangeState(new StateResult(context));
            context.equal();
        }

        public void opers(string key)
        {
            context.ChangeState(new StateOpers(context));
            context.opers(key);
        }

        public string scren()
        {
            return context.x.ToString();
        }
    }
}