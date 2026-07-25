**TASK 1 Solution Notes**

For this task, I implemented the load balancing setup using ASP.NET/.NET, since I am more familiar with this framework. The load balancing algorithms were configured inside the nginx.conf file.

I created three server instances and configured them using launchSettings.json, where each instance runs on a different port. I also defined the server names inside appsettings.json so that each instance could return a unique response identifying which server handled the request.

The three backend servers were started simultaneously, and I tested the setup using both Postman and PowerShell.

**Round Robin Testing**

Initially, I tested the default Round Robin algorithm in Nginx. The expected behavior was:

Server 1 → Server 2 → Server 3 → Server 1 → Server 2 → Server 3

However, the observed behavior was:

Server 1 → Server 1 → Server 2 → Server 2 → Server 3 → Server 3

The reason appears to be related to connection reuse/keep-alive behavior, where multiple requests were being sent through the same connection before Nginx switched to another backend server. I attempted to modify Nginx settings, including changing worker_processes from auto to 1, but this did not change the observed behavior. Therefore, I continued with the experiment while documenting this difference.

Fault Tolerance Testing

After running the request loop, I stopped one of the server instances (Server 2) while requests were still being sent.

The observed behavior was that Nginx stopped routing requests to the unavailable server and continued sending requests to the remaining healthy servers:

Server 1 → Server 3 → Server 1 → Server 3

This demonstrated the concept of fault tolerance, where the system can continue operating even when one backend instance fails.

**IP Hash Testing**

Finally, I changed the load balancing algorithm to IP Hash by modifying the Nginx configuration.

With IP Hash enabled, requests from the same client were consistently routed to the same backend server. In my test, all requests were directed to Server 1.

This behavior can be useful when an application stores user session data locally on the server. For example, after a user logs in, their session information may be stored inside Server 1. Future requests from the same user will continue reaching Server 1, allowing the session to be maintained.

However, this approach introduces a problem if Server 1 fails. The user's requests may be redirected to Server 2, but Server 2 will not have access to the session data stored on Server 1, causing the user session to be lost.

A better approach is to store session data in a shared external storage system, such as a database or Redis. This allows any server instance to access the same session information, making the system more scalable and reliable.
