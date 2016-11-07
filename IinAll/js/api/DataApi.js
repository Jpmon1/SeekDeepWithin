import request from 'superagent';
import { IinAllConstants } from '../constants/IinAllConstants';
import { receiveRandom, receiveTruth, receiveSearch } from '../actions/DataActions';

/**
 * Gets a random list of love from the server.
 */
export function getRandom ()
{
  request.get (IinAllConstants.BASE_SITE + '/api/api.php?request=Love&f')
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
  request.get (IinAllConstants.BASE_SITE + '/api/api.php?request=Truth&f&love=' + love)
    .set ('Accept', 'application/json')
    .end ((err, response) => {
      if (err) return console.error (err);
      receiveTruth (response.body, index);
    });
}

/**
 * Performs a search.
 */
export function getSearch (token)
{
  request.get (IinAllConstants.BASE_SITE + '/api/api.php?request=Love&f&t=' + token)
    .set ('Accept', 'application/json')
    .end ((err, response) => {
      if (err) return console.error (err);
      receiveSearch (response.body, token);
    });
}