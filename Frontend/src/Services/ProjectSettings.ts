import { http } from "@/config/ApiClient"
import type {
    AddStatus,
    EditStatus,
    IStatus,
    AddClient,
    EditClient,
    IClient,
    RuleBook,
} from "@/Models/ProjectSettings"

export class ProjectSettingsService {
    private readonly baseUrl = '/ProjectSettings'

    public GetAllStatus = async (projectId: string): Promise<IStatus[] | null> =>
        await http.get(`${this.baseUrl}/Status/${projectId}`)

    public AddStatus = async (addStatus: AddStatus): Promise<IStatus | null> =>
        await http.post(`${this.baseUrl}/Status`, addStatus)

    public UpdateStatus = async (editStatus: EditStatus): Promise<IStatus | null> =>
        await http.put(`${this.baseUrl}/Status`, editStatus)

    public DeleteStatus = async (id: string): Promise<boolean | null> =>
        await http.delete(`${this.baseUrl}/Status/${id}`)

    public GetAllClient = async (projectId: string): Promise<IClient[] | null> =>
        await http.get(`${this.baseUrl}/Client/${projectId}`)

    public AddClient = async (addClient: AddClient): Promise<IClient | null> =>
        await http.post(`${this.baseUrl}/Client`, addClient)

    public UpdateClient = async (editClient: EditClient): Promise<IClient | null> =>
        await http.put(`${this.baseUrl}/Client`, editClient)

    public DeleteClient = async (id: string): Promise<string | null> =>
        await http.delete(`${this.baseUrl}/Client/${id}`)

    public UpdateRuleBook = (rule: RuleBook): Promise<RuleBook | null> =>
        http.put(`${this.baseUrl}/RuleBook`, rule)

    public GetRuleBook = (projectId: string): Promise<RuleBook | null> =>
        http.get(`${this.baseUrl}/RuleBook/${projectId}`)
}