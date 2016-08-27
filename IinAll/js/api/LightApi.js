import request from 'superagent';
import { receiveRandom, receiveFormat } from '../actions/LightServerActions';

/**
 * Gets a random list of light from the server.
 */
export function getRandomApi ()
{
  request.get ('/IinAllDev/api/api.php?request=Light')
    .set ('Accept', 'application/json')
    .end ((err, response) => {
      if (err) return console.error (err);
        receiveRandom (response.body);
    });
}