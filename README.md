# API_MVP

## Install/Configure
To install/configure the API:
- Clone the repository.
- Open IIS and add a new web site.
- Set the physical path to this: {yourClonedRepoPath}\Deploy\v1<br/>
For example, if you cloned your repository to C:\OffToSomePlace\GitHub\API_MVP\, the physical path would look like this: C:\OffToSomePlace\GitHub\API_MVP\Deploy\v1
- Set the port to an open port.
- Click OK.
- Navigate to http://localhost:{theportyouset}/swagger/index.html

## API Help
Once your API is being served, an interactive UI will be avaiable for the API at: http://{baseUrl}/swagger/index.html<br/>
For example: http://localhost:8089/swagger/index.html<br />
- All information needed for this API will be provided here.
- Subsequent versions will also be available here as they are released.

<strong>NOTE</strong>: Any API requests you make here will update the database.