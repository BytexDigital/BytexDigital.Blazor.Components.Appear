function observe(elementSelector, componentRef, appearMethod, disappearMethod, margins, thresholds) {
    var observer = new IntersectionObserver(function (entries) {
        if (entries[0]['isIntersecting'] === true) {
            componentRef.invokeMethodAsync(appearMethod, entries[0].intersectionRatio);
        }
        else {
            componentRef.invokeMethodAsync(disappearMethod, 0);
        }
    }, {
        threshold: thresholds,
        rootMargin: margins
    });

    observer.observe(document.querySelector(elementSelector));
};

export { observe }