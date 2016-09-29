import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/github";

class GitHubApiService {
    constructor() {

    }

    async getFollowingList() {
        return await getAsync(endpoint, 'following', AuthService.accessToken);
    }
}

export default new GitHubApiService()