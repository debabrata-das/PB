## Technical questions

1. What architectures or patterns are you using currently or have worked on recently?

   My day-job involves maintaining a monolith (which is basically a client-server architecture with multiple dependent layers in it) however we do use normal  patterns like  Singletons (and try to follow Clean code principles normally along the way). I have also tried learning and working with the DDD pattern recently, along with event-driven architecture.

2. What do you think of them and would you want to implement it again?

   Maintaining a monolith has its challenges however it is not entirely un-workable. Given a choice I would indeed like to replace it with a more micro-services oriented architecture flow, also with a focus on making the whole application more testable by adhering to TDD (or BDD) way of software development.

3. What version control system do you use or prefer?

   I have used both Git & Svn however Git would be my preference.

4. What is your favorite language feature and can you give a short snippet on how you use it?

   C# - I like the (more functional) direction it is going making it more easier write good quality code with it.
   For example - switch expression 
   
```
public enum Rainbow
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet
}

public static RGBColor FromRainbow(Rainbow colorBand) =>
    colorBand switch
    {
        Rainbow.Red    => new RGBColor(0xFF, 0x00, 0x00),
        Rainbow.Orange => new RGBColor(0xFF, 0x7F, 0x00),
        Rainbow.Yellow => new RGBColor(0xFF, 0xFF, 0x00),
        Rainbow.Green  => new RGBColor(0x00, 0xFF, 0x00),
        Rainbow.Blue   => new RGBColor(0x00, 0x00, 0xFF),
        Rainbow.Indigo => new RGBColor(0x4B, 0x00, 0x82),
        Rainbow.Violet => new RGBColor(0x94, 0x00, 0xD3),
        _              => throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand)),
    };

```

5. What future or current technology do you look forward to the most or want to use and why?

   I would like to work more with GraphQL and also Python (AI). I think find GraphQL is the next clear logical step for working with the microservice architecture. 
   And I think in near future all our jobs would involve some amount of Python programming due to the insurgence of AI related software which impacts almost all domains.

6. How would you find a production bug/performance issue? Have you done this before?

   Yes, all the time. Normally we use Kibana for checking application logs from Production (not directly), and, we also use Prometheus with Grafana dashboards to monitor our application state (not directly) so we get notified pretty much quickly when something is wrong. 
   The most recent example would be removing millions of redundant logging that was causing a performance bottleneck and also causing issue in getting debugging information.

7. How would you improve the application (bug fixes, security, performance, etc.)?

   Add sorting, filtering, and pagination support for the GET methods
   Use LogStash and push the logs generated from log4net to a Kibana instance
   Add more integration & unit tests to expand the test coverage
   Add more metrics using prometheus and create a grafana dashboard
   Add some more documentation about the code and its workings
   Dockerize the application
   
