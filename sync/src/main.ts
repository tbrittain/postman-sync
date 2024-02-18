import fs from 'fs'
import * as path from "path";

function loadEnv() {
    const defaultPath = path.join(__dirname, '../.env')

    if (!fs.existsSync(defaultPath)) {
        console.error(`.env file not found at ${defaultPath}`)
        process.exit(1)
    }

    require('dotenv')
        .config({path: defaultPath})
}

async function main() {
    loadEnv()

    const url = process.env.SWAGGER_JSON_URL
    if (!url) {
        console.error('SWAGGER_JSON_URL not found in .env')
        process.exit(1)
    }

    console.log(`SWAGGER_JSON_URL: ${url}`)

    process.exit(0)
}

main()