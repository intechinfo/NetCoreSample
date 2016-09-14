import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/class";

class ClassApiService {

  constructor(token) {
      this.token = token;
  }

  async getClassListAsync() {
      return await getAsync(endpoint, '', this.token);
  }

  async getClassAsync(classId) {
      return await getAsync(endpoint, classId, this.token);
  }

  async createClassAsync(model) {
      return await postAsync(endpoint, '', this.token, model);
  }

  async updateClassAsync(model) {
      return await putAsync(endpoint, model.classId, this.token, model);
  }

  async deleteClassAsync(classId) {
      return await deleteAsync(endpoint, classId, this.token);
  }
}

export default ClassApiService