import fs from 'fs'
import * as path from "path";
import {convert} from "./convert";

function loadEnv() {
    const defaultPath = path.join(__dirname, '../.env')

    if (!fs.existsSync(defaultPath)) {
        console.error(`.env file not found at ${defaultPath}`)
        process.exit(1)
    }

    require('dotenv')
        .config({path: defaultPath})
}

async function retrieveSwaggerJson(url: string) {
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

async function main() {
    loadEnv()

    const url = process.env.SWAGGER_JSON_URL
    if (!url) {
        console.error('SWAGGER_JSON_URL not found in .env')
        process.exit(1)
    }

    const json = await retrieveSwaggerJson(url)
    const result = await convert(json)

    console.log(result)

    process.exit(0)
}

main()