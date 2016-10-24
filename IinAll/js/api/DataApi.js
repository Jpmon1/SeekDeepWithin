import request from 'superagent';
import { IinAllConstants } from '../constants/IinAllConstants';
import { receiveRandom, receiveTruth } from '../actions/DataActions';

/**
 * Gets a random list of love from the server.
 */
export function getRandom ()
{
  request.get (IinAllConstants.BASE_SITE + '/api/api.php?request=Random')
    .set ('Accept', 'application/json')
    .end ((err, response) => {
      if (err) return console.error (err);
        receiveRandom (response.body);
    });
}

/**
 * Gets the truth for the given love.
 */
export function getTruth (love, index)
{
  request.get (IinAllConstants.BASE_SITE + '/api/api.php?request=Truth&love=' + love)
    .set ('Accept', 'application/json')
    .end ((err, response) => {
      if (err) return console.error (err);
        receiveTruth (response.body, index);
    });
}