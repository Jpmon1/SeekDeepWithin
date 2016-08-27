import AppDispatcher from '../dispatchers/IinAllDispatcher';
import { IinAllConstants } from '../constants/IinAllConstants';
import { getRandomApi } from '../api/LightApi';

/**
 * Gets a random list of light from the server.
 */
export function getRandom ()
{
   AppDispatcher.handleViewAction ({
      actionType: IinAllConstants.GET_RANDOM
   });
   getRandomApi ();
}
