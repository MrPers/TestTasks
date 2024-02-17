namespace Calculator
{
    internal class StateOpers : State
    {
        private Context context;

        public StateOpers(Context context)
        {
            this.context = context;
            context.op = "";
        }

        public void digit(string key)
        {
            context.ChangeState(new StateSecond(context));
            context.digit(key);
        }

        public void equal()
        {
            context.y = context.x;
            context.ChangeState(new StateResult(context));
            context.equal();
        }

        public void opers(string key)
        {
            context.op = key;
        }

        public string scren()
        {
            return context.x.ToString() + " " + context.op;
        }
    }
}