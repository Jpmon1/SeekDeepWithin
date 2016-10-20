import AppDispatcher from '../dispatchers/IinAllDispatcher';
import { IinAllConstants } from '../constants/IinAllConstants';
import { getRandom, getTruth } from '../api/DataApi';

/**
 * Gets a random list of love from the server.
 */
export function requestRandom ()
{
   AppDispatcher.handleViewAction ({
      actionType: IinAllConstants.GET_RANDOM
   });
   getRandom ();
}

/**
 * Receives a random list of love from the server.
 */
export function receiveRandom (response) {
  AppDispatcher.handleServerAction({
    actionType: IinAllConstants.GET_RANDOM_RESPONSE,
    response: response,
  });
}

/**
 * Requests truth from the server.
 */
export function requestTruth (love, index) {
  AppDispatcher.handleServerAction({
    actionType: IinAllConstants.GET_TRUTH
  });
  getTruth (love, index);
}

/**
 * Receives truth from the server.
 */
export function receiveTruth (response, index) {
  AppDispatcher.handleServerAction({
    actionType: IinAllConstants.GET_TRUTH_RESPONSE,
    response: response,
    index: index
  });
}
