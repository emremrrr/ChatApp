using System;

namespace ChatApp.Queue.Services;

public class SessionService
{
    public Queue<Guid> SessionQueue ;
    public SessionService()
    {
        SessionQueue=new Queue<Guid>();
    }
    
}
