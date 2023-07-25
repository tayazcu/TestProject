
import { UrlConfigService } from "../../services/url-config.service";
export function configurationFactory(urlConfig: UrlConfigService
  ): ()=>Promise<any> {
          return ():Promise<any>=>{
                 return new Promise<void>((resolve, reject)=>{
                        const configDeps:Promise<any>[]=[];
                        urlConfig.load()
                    .then((config:any)=>{
                          Promise.all(configDeps)
                          .then(()=>{
                                resolve();
                                    }).catch(()=>{
                                          reject();
                                      });
                                    })
                                 })
                               }
                             }
