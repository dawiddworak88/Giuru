import React, { useRef, useContext, useState, useCallback } from "react";
import PropTypes from "prop-types";
import ResponsiveImage from "../../../../../../shared/components/Picture/ResponsiveImage";
import useDynamicSearch from "../hooks/useDynamicSearch";
import { Context } from "../../../../../../shared/stores/Store";

const NewsCatalog = (props) => {
    const [state] = useContext(Context);
    const [pageIndex, setPageIndex] = useState(1)

    const {
        news, hasMore
    } = useDynamicSearch(props.newsApiUrl, null, 10, 1)

    const observer = useRef()
    const lastElement = useCallback(node => {
        if (loading) return
        if (observer.current) observer.current.disconnect()
            observer.current = new IntersectionObserver(entries => {
                if (entries[0].isIntersecting && hasMore) {
                    setPageIndex(prevPageIndex => prevPageIndex + 1)
                }
            })

        if (node) observer.current.observe(node)
    }, [state, hasMore])

    console.log(news)
    return (
        <section className="section">
            <div class="container">
                
            </div>

            
                     <div class="columns is-vcentered">
                        <div class="column is-8">
                            <figure class="image is-16by9">
                                <img src="https://picsum.photos/1200/600/?random" alt="Description" />
                            </figure>
                        </div>
                        <div class="column">
                            <h1 class="title is-2">
                                Superhero Scaffolding
                            </h1>
                            <h2 class="subtitle is-4">
                                Let this cover page describe a product or service.
                            </h2>
                            <br />
                            <p class="has-text-centered">
                                <a class="button is-medium is-info is-outlined">
                                    Learn more
                                </a>
                            </p>
                        </div>
                    </div>
             
        </section>

    //      <section className="section">
    //         <div className="columns">
    //             <div className="row columns is-multiline">
    //                 {news.slice(1).map((newsItem, i) => {
    //                 return (
    //                     <div class="card">
    //                         <div class="card-image">
    //                             <figure class="image is-4by3">
    //                                 <img src="https://bulma.io/images/placeholders/1280x960.png" alt="Placeholder image"/>
    //                             </figure>
    //                         </div>
    //                         <div class="card-content">
    //                             <div class="media">
    //                                 <div class="media-left">
    //                                     <figure class="image is-48x48">
    //                                         <img src="https://bulma.io/images/placeholders/96x96.png" alt="Placeholder image"/>
    //                                     </figure>
    //                                 </div>
    //                                 <div class="media-content">
    //                                     <p class="title is-4">John Smith</p>
    //                                     <p class="subtitle is-6">@johnsmith</p>
    //                                 </div>
    //                             </div>

    //                             <div class="content">
    //                             Lorem ipsum dolor sit amet, consectetur adipiscing elit.
    //                             Phasellus nec iaculis mauris. <a>@bulmaio</a>.
    //                             <a href="#">#css</a> <a href="#">#responsive</a>
    //                             <br/>
    //                                 <time datetime="2016-1-1">11:09 PM - 1 Jan 2016</time>
    //                             </div>
    //                         </div>
    //                     </div>
    //                 )
    //             })}
    //             </div>
    //         </div>
    //     </section> 
    //          <div class="container">
    //   <div class="section">
    //     <div class="columns">
    //       <div class="column has-text-centered">
    //         <h1 class="title" style="color: ghostwhite;">Bulma Card Layout Template</h1><br/>
    //       </div>
    //     </div>
    //     <div id="app" class="row columns is-multiline">
    //       <div class="column is-4">
    //         <div class="card large">
    //           <div class="card-image">
    //             <figure class="image is-16by9">
    //               <img src="https://picsum.photos/1200/600/?random" alt="Image"/>
    //             </figure>
    //           </div>
    //           <div class="card-content">
    //             <div class="media">
    //               <div class="media-left">
    //                 <figure class="image is-48x48">
    //                   <img src="https://picsum.photos/1200/600/?random" alt="Image"/>
    //                 </figure>
    //               </div>
    //               <div class="media-content">
    //                 <p class="title is-4 no-padding">asd</p>
    //                 <p>
    //                   <span class="title is-6">
    //                     <a href="as"> asd </a> </span> </p>
    //                 <p class="subtitle is-6">asd</p>
    //               </div>
    //             </div>
    //             <div class="content">
    //              sasdasd
    //               <div class="background-icon"><span class="icon-twitter"></span></div>
    //             </div>
    //           </div>
    //         </div>
    //       </div>
    //     </div>
    //   </div>
    // </div> 

    )
}

NewsCatalog.propTypes = {
    
};

export default NewsCatalog;