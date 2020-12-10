There are lots of caching solutions out there - memcache, redis, azure cache etc. 
All of them require extra money or extra servers. 
However RAM is cheap and you probably are not using all of it at the moment anyway. 
Unless you know upfront you need a massive cache I recommend starting with LazyCache because it is so quick to get started. 

You can always change the underlying cache provider later on without breaking your application.