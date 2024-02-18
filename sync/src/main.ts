import fs from 'fs'
import * as path from "path";
import {convert} from "./convert";
import {pushCollectionToPostman} from "./push";
import {getExistingPostmanCollection, retrieveSwaggerJson} from "./fetch";
import {logger} from "./logger";

function loadEnv() {
    const defaultPath = path.join(__dirname, '../.env')

    if (!fs.existsSync(defaultPath)) {
        logger.error(`.env file not found at %s`, defaultPath)
        process.exit(1)
    }

    require('dotenv')
        .config({path: defaultPath})
}

async function main() {
    loadEnv()

    const url = process.env.SWAGGER_JSON_URL
    if (!url) {
        logger.error('SWAGGER_JSON_URL not found in .env')
        process.exit(1)
    }

    logger.info('Retrieving swagger json from %s', url)
    const json = await retrieveSwaggerJson(url)
    logger.info('Converting swagger json to postman collection metadata')
    const collection = await convert(json)

    const existingCollectionId = process.env.POSTMAN_COLLECTION_ID
    if (existingCollectionId) {
        logger.info('Updating existing collection with id %s', existingCollectionId)
        const existingCollection = await getExistingPostmanCollection(existingCollectionId)

        // TODO: Determine the merge strategy to use here, for now we'll just overwrite
        if (!existingCollection) {
            logger.error('Failed to retrieve existing collection')
            process.exit(1)
        }

        // TODO: Here, call the PUT and update the collection
    } else {
        logger.info('Creating new collection')
        await pushCollectionToPostman(collection)
    }

    logger.info('Done')
    process.exit(0)
}

main()