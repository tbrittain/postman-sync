import Converter, {Input} from "openapi-to-postmanv2";
import util from "node:util";

const promise = util.promisify(Converter.convert);

export async function convert(json: string) {
  const input: Input = {
    type: 'json',
    data: json,
  }

  const result = await promise(input, undefined);
  if (!result.result) {
    throw new Error(`Failed to convert: ${result.reason}`);
  }

  return result;
}