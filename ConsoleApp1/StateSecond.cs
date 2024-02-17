namespace Calculator
{
    internal class StateSecond : State
    {
        private Context context;

        public StateSecond(Context context)
        {
            this.context = context;
            context.y = 0;
        }

        public void digit(string key)
        {
            int digit = Convert.ToInt32(key);
            context.y = context.y * 10 + digit;
        }

        public void equal()
        {
            context.ChangeState(new StateResult(context));
            context.equal();
        }

        public void opers(string key)
        {
            context.ChangeState(new StateResult(context));
            context.equal();
        }

        public string scren()
        {
            return context.op +" "+ context.y;
        }
    }
}