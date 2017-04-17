import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/github";

class GitHubApiService {
    constructor() {

    }

    async getFollowingList() {
        return await getAsync(`${endpoint}/following`);
    }
}

export default new GitHubApiService()