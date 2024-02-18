import Converter, {Input} from "openapi-to-postmanv2";
import util from "node:util";
import {Collection} from "postman-collection";

const promise = util.promisify(Converter.convert);

export async function convert(json: string): Promise<Collection> {
  const input: Input = {
    type: 'json',
    data: json,
  }

  const result = await promise(input, undefined);
  if (!result.result) {
    throw new Error(`Failed to convert: ${result.reason}`);
  }

  return new Collection(result.output[0].data)
}