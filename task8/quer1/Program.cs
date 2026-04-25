using System;
namespace que1{
    struct Model{
        public string Message{get;set;}
        ///Дата отправки
        public DateTime DispatchDate{get;set;}
        public DateTime ProcessingDate{get;set;}
        public int ClientID{get;set;}
    }
    class Client{
        public int ID{get;}
         private  System.Timers.Timer timer;
private byte[]tmp;
private Random rand;
        public Client(int id){
            ID=id;
            timer=new  System.Timers.Timer(100);
            rand=new Random();
            timer.Elapsed+=(source,e)=>{
var m=new Model();
m.ClientID=ID;
m.Message=Convert.ToBase64(tmp);
m.DispatchDate=DateTime.Now;
Program.stack.Push(m);
            };

        }
        public Task Run(){
            return Task.Run(()=>{
timer.AutoReset = true;
timer.Enabled=true;

            });

        }
        public void AcceptAnObject(Model m){
            Console.WriteLine($"client {ID} accept message {m.Message} in {m.ProcessingDate}");
        }
        public Task Wait(){
            return Task.Run(()=>{
                timer.Stop();
            });
        }
    }
    class Program{
        public static ConcurrentStack<Model>stack=new ConcurrentStack<Model>();
        static void Main(string[]args){
var clients=new Client[2];
Task.Run(async()=>{
for(int l=0;l<clients.Length;l++){
    clients[i]=new Client(i);
    await clients[i].Run();
}
});
System.Timers.Timer timer=new System.Timers.Timer(500);
timer.Elapsed+=(sender,e)=>{
while(stack.TryPop(out f_)){
f_.ProcessingDate=DateTime.Now;
/*
https://learn.microsoft.com/ru-ru/dotnet/api/system.collections.concurrent.concurrentstack-1?view=net-10.0
*/
clients[f_.ID].AcceptAnObject(f_);
}
};
        }
    }
}
