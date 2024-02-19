import {Collection} from "postman-collection";
import {logger} from "./logger";

type CollectionResponse = {
  collection: {
    id: string;
    uid: string;
    name: string;
  }
}

export async function pushCollectionToPostman(collection: Collection, existingCollectionId: string | undefined = undefined) {
  const postmanApiKey = process.env.POSTMAN_API_KEY;
  if (!postmanApiKey) {
    logger.crit({
      message: "POSTMAN_API_KEY not found in .env"
    });
    process.exit(1);
  }

  const method = existingCollectionId ? "PUT" : "POST";
  let url = "https://api.getpostman.com/collections";
  if (existingCollectionId) {
    url += `/${existingCollectionId}`;
  }

  logger.debug({
    message: "Pushing collection to Postman",
    url,
    method
  })

  const response = await fetch(url, {
    method,
    headers: {
      "Content-Type": "application/json",
      "X-Api-Key": postmanApiKey,
    },
    body: JSON.stringify({
      collection: collection.toJSON()
    }),
  });

  if (!response.ok) {
    const body = await response.text();
    logger.error({
      message: "Failed to create collection",
      status: response.status,
      statusText: response.statusText,
      body
    })
    throw new Error(`Failed to create collection: ${response.status} ${response.statusText}`);
  }

  const responseData = await response.json() as CollectionResponse;
  logger.info({
    message: "Collection created",
    collection: responseData.collection
  })
}