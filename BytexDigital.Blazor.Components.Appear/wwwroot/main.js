function observe(elementSelector, componentRef, appearMethod, disappearMethod, margins) {
    var observer = new IntersectionObserver(function (entries) {
        if (entries[0]['isIntersecting'] === true) {
            componentRef.invokeMethodAsync(appearMethod);
        }
        else {
            componentRef.invokeMethodAsync(disappearMethod);
        }
    }, {
        threshold: [0],
        rootMargin: margins
    });

    observer.observe(document.querySelector(elementSelector));
};

export { observe }