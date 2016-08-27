import AppDispatcher from '../dispatchers/IinAllDispatcher';
import { IinAllConstants } from '../constants/IinAllConstants';

/**
 * Receives a random list of light from the server.
 */
export function receiveRandom (response) {
  AppDispatcher.handleServerAction({
    actionType: IinAllConstants.GET_RANDOM_RESPONSE,
    response: response,
  });
}