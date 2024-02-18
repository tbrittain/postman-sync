import {Collection} from "postman-collection";
import {logger} from "./logger";

export async function retrieveSwaggerJson(url: string) {
  const response = await fetch(url, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    }
  })
  if (!response.ok) {
    throw new Error(`Failed to fetch ${url}: ${response.status} ${response.statusText}`)
  }

  // grab the string representation of the json rather than a json object
  return await response.text()
}

export async function getExistingPostmanCollection(collectionId: string) {
  const postmanApiKey = process.env.POSTMAN_API_KEY
  if (!postmanApiKey) {
    logger.error('POSTMAN_API_KEY not found in .env')
    process.exit(1)
  }

  const response = await fetch(`https://api.getpostman.com/collections/${collectionId}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-Api-Key': postmanApiKey
    }
  })
  if (!response.ok) {
    throw new Error(`Failed to fetch collection: ${response.status} ${response.statusText}`)
  }

  // TODO: This may not be the correct type
  const collection = (await response.json()).collection as Collection;
  return collection
}