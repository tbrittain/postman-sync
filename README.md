# postman-sync

Postman Sync is a tool that takes in a Swagger JSON specification, converts it to a Postman specification, and then uploads it to a new or existing Postman collection.

There is [a way within Postman](https://learning.postman.com/docs/getting-started/importing-and-exporting/importing-from-swagger/) to import a Swagger JSON file, but it is a manual process. This tool automates that process.

## Usage

- Annotate C# types with Swagger attributes ([including exporting the XML doc as part of the build](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio#xml-comments))
- Have the backend running in an environment where the Swagger JSON is accessible
  - For example, if the backend is running locally, the Swagger JSON will be at `http://localhost:5299/swagger/v1/swagger.json`
- Configure a `.env` file with the following variables:
  - `POSTMAN_API_KEY`: Your Postman API key from the [Postman website](https://web.postman.co/settings/me/api-keys)
  - `SWAGGER_JSON_URL`: The URL to the Swagger JSON file
  - `POSTMAN_COLLECTION_ID`: The ID of the Postman collection to update. If the collection does not yet exist, do not include this variable
- Run `npm run build` and `npm run start`

### Helpful links



Postman Collection JS SDK:
http://www.postmanlabs.com/postman-collection/

Postman API docs:
https://www.postman.com/postman/workspace/postman-public-workspace/documentation/12959542-c8142d51-e97c-46b6-bd77-52bb66712c9a

Postman Collection API docs:
https://www.postman.com/postman/workspace/postman-public-workspace/folder/12959542-c705956d-1005-4fbc-803c-b6b985242a85