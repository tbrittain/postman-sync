import {Collection} from "postman-collection";
import {logger} from "./logger";

type CollectionResponse = {
  collection: {
    id: string;
    uid: string;
    name: string;
  }
}

export async function pushCollectionToPostman(collection: Collection) {
  const postmanApiKey = process.env.POSTMAN_API_KEY;
  if (!postmanApiKey) {
    logger.error("POSTMAN_API_KEY not found in .env");
    process.exit(1);
  }

  const response = await fetch("https://api.getpostman.com/collections", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "X-Api-Key": postmanApiKey,
    },
    body: JSON.stringify({
      collection: collection.toJSON()
    }),
  });

  if (!response.ok) {
    throw new Error(`Failed to create collection: ${response.status} ${response.statusText}`);
  }

  const responseData = await response.json() as CollectionResponse;
  logger.info({
    message: "Collection created",
    collection: responseData.collection
  })
}