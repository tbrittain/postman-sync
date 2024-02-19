import fs from "fs";
import * as path from "path";
import { convert } from "./convert";
import { pushCollectionToPostman } from "./push";
import { getExistingPostmanCollection, retrieveSwaggerJson } from "./fetch";
import { logger } from "./logger";

function loadEnv() {
  const defaultPath = path.join(__dirname, "../.env");

  if (!fs.existsSync(defaultPath)) {
    logger.crit({
      message: "No .env file found",
      path: defaultPath,
    });
    process.exit(1);
  }

  require("dotenv").config({ path: defaultPath });
}

async function main() {
  loadEnv();

  const url = process.env.SWAGGER_JSON_URL;
  if (!url) {
    logger.crit({
      message: "SWAGGER_JSON_URL not found in .env",
    });
    process.exit(1);
  }

  logger.info({
    message: "Retrieving swagger json from url",
    url,
  });
  const json = await retrieveSwaggerJson(url);
  logger.info({
    message: "Converting swagger json to postman collection metadata",
  });
  const collection = await convert(json);

  const existingCollectionId = process.env.POSTMAN_COLLECTION_ID;
  if (existingCollectionId) {
    logger.info({
      message: "Updating existing collection",
      id: existingCollectionId,
    });
    const existingCollection =
      await getExistingPostmanCollection(existingCollectionId);

    if (!existingCollection) {
      logger.error({
        message: "Collection not found",
        id: existingCollectionId,
      });
      process.exit(1);
    }

    await pushCollectionToPostman(collection, existingCollectionId);
  } else {
    logger.info({
      message: "Creating new collection",
    });
    await pushCollectionToPostman(collection);
  }

  logger.info({ message: "Done" });
  process.exit(0);
}

main();
