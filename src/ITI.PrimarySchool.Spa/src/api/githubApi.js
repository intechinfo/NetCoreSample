import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/github";

export async function getFollowingList() {
    return await getAsync(`${endpoint}/following`);
}